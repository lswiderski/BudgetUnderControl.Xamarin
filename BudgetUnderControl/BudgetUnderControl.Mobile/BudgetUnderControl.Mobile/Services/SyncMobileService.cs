﻿using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class SyncMobileService : ISyncMobileService
    {
        private static string BACKUP_FILE_NAME = "buc_backup.json";

        private readonly IFileHelper fileHelper;
        private readonly ISyncService syncService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ISyncRequestBuilder syncRequestBuilder;
        private readonly ISynchroniser synchroniser;
        private readonly HttpClient httpClient;
        public SyncMobileService(IFileHelper fileHelper,
            ISyncService syncService,
            ITransactionRepository transactionRepository,
            ISyncRequestBuilder syncRequestBuilder,
            ISynchroniser synchroniser
            )
        {
            this.fileHelper = fileHelper;
            this.syncService = syncService;
            this.transactionRepository = transactionRepository;
            this.syncRequestBuilder = syncRequestBuilder;
            this.synchroniser = synchroniser;
            httpClient = App.Container.ResolveNamed<HttpClient>("api");
        }


        public async Task<string> GetBackUpJSONAsync()
        {
            var backUp = await this.syncService.GetBackUpAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(backUp);
            return output;
        }

        private async Task ImportBackUpJSONAsync(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            try
            {
                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<BackUpDTO>(json);

                await syncService.ImportBackUpAsync(deserialized);
            }
            catch (Exception e)
            {
                //Problem with JSON file
                throw;
            }

        }

        public async Task SaveBackupFileAsync()
        {
            var json = await GetBackUpJSONAsync();

            fileHelper.SaveText(BACKUP_FILE_NAME, json);
        }

        public async Task LoadBackupFileAsync()
        {
            var json = fileHelper.LoadText(BACKUP_FILE_NAME);
            await ImportBackUpJSONAsync(json);
        }

        public async Task ExportCSVAsync()
        {
            var fileName = string.Format("{0}_{1}.txt", "buc", DateTime.UtcNow.Ticks);
            var lines = await syncService.GenerateCSV();
            fileHelper.SaveText(fileName, lines.ToArray());
        }

        public async Task ExportDBAsync()
        {
            //temporary
            ExtractDB();
        }

        public void ExtractDB()
        {
            var sourcePath = fileHelper.GetLocalFilePath(Settings.DB_SQLite_NAME);

            var databaseBackupPath = fileHelper.GetExternalFilePath(string.Format("{0}_{1}.db3", "buc_Backup", DateTime.UtcNow.Ticks));
            fileHelper.CopyFile(sourcePath, databaseBackupPath);

        }

        public async Task SyncAsync()
        {
            //get request
            var syncRequestDto = await this.syncRequestBuilder.CreateSyncRequestAsync(SynchronizationComponent.Mobile, SynchronizationComponent.Api);

            //call api
            try
            {
                var url = "sync/sync";
                var dataAsString = JsonConvert.SerializeObject(syncRequestDto);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var json = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var apiResponse = serializer.Deserialize<SyncRequest>(json);

                    //do sync with responsedto
                    await this.synchroniser.SynchroniseAsync(apiResponse);
                }
            }
            catch (Exception e)
            {
                //just for development purpose
                throw e;
            }
            

        }
    }
}
