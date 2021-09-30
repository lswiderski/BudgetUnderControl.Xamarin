using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class FileRepository : BaseModel, IFileRepository
    {
        private readonly IUserIdentityContext userIdentityContext;
        private readonly IFileHelper fileHelper;

        public FileRepository(IContextFacade context,
             Func<IUserIdentityContext> userIdentityContextFunc,
            IFileHelper fileHelper) : base(context)
        {
            this.userIdentityContext = userIdentityContextFunc();
            this.fileHelper = fileHelper;
        }

        public async Task AddAsync(File file)
        {
            await this.Context.Files.AddAsync(file);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddAsync(List<File> files)
        {
            await this.Context.Files.AddRangeAsync(files);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<File>> GetAsync()
        {
            var list = await this.Context.Files
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<ICollection<File>> GetAsync(List<int> fileIds)
        {
            var list = await this.Context.Files
                .Where(t => fileIds.Contains(t.Id))
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<ICollection<File>> GetAsync(List<string> fileIds)
        {
            var list = await this.Context.Files
                .Where(t => fileIds.Contains(t.ExternalId))
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<File> GetAsync(int id)
        {
            var file = await this.Context.Files
                .FirstOrDefaultAsync(x => x.Id == id);

            return file;
        }

        public async Task<File> GetAsync(string id)
        {
            var file = await this.Context.Files.FirstOrDefaultAsync(x => x.ExternalId == id);

            return file;
        }

        public async Task UpdateAsync(File file)
        {
            this.Context.Files.Update(file);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(File file)
        {
            var now = DateTime.UtcNow;
            var f2ts = await this.Context.FilesToTransactions.Where(t => t.FileId == file.Id).ToListAsync();
            foreach (var f2t in f2ts)
            {
                f2t.Delete();
            }
            file.Delete();
            await this.fileHelper.TaskRemoveFileAsync(file.ExternalId);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(IEnumerable<File> files)
        {
            var fileIds = files.Select(x => x.Id).ToList();
            var f2t = await this.Context.FilesToTransactions.Where(t => fileIds.Contains(t.FileId)).ToListAsync();
            await this.RemoveAsync(f2t);
            foreach (var file in files)
            {
                await this.fileHelper.TaskRemoveFileAsync(file.ExternalId);
            }
            this.Context.Files.RemoveRange(files);
            await this.Context.SaveChangesAsync();
        }

        public async Task<FileToTransaction> GetFileToTransactionAsync(int fileToTransactionId)
        {
            var fileToTransaction = await this.Context.FilesToTransactions.FirstOrDefaultAsync(x => x.Id == fileToTransactionId);
            return fileToTransaction;
        }

        public async Task<ICollection<FileToTransaction>> GetFileToTransactionsAsync()
        {
            var list = await this.Context.FilesToTransactions.ToListAsync();
            return list;
        }

        public async Task<ICollection<FileToTransaction>> GetFileToTransactionsAsync(int transactionId)
        {
            var list = await this.Context.FilesToTransactions
                .Where(t => t.TransactionId == transactionId)
                .ToListAsync();
            return list;
        }

        public async Task AddAsync(FileToTransaction fileToTransaction)
        {
            await this.Context.FilesToTransactions.AddAsync(fileToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<FileToTransaction> filesToTransaction)
        {
            await this.Context.FilesToTransactions.AddRangeAsync(filesToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FileToTransaction fileToTransaction)
        {
            this.Context.FilesToTransactions.Update(fileToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(FileToTransaction fileToTransaction)
        {
            if(fileToTransaction != null)
            {
                await this.fileHelper.TaskRemoveFileAsync(fileToTransaction.File.ExternalId);
                fileToTransaction.Delete();
                fileToTransaction.File.Delete();
                await this.Context.SaveChangesAsync();
            }
           
        }

        public async Task RemoveAsync(IEnumerable<FileToTransaction> filesToTransaction)
        {
            if(filesToTransaction != null)
            {
                var files = filesToTransaction.Select(x => x.File).ToList();
                this.Context.FilesToTransactions.RemoveRange(filesToTransaction);

                foreach (var file in files)
                {
                    await this.fileHelper.TaskRemoveFileAsync(file.ExternalId);
                }
                this.Context.Files.RemoveRange(files);
                await this.Context.SaveChangesAsync();
            }
            
        }
    }
}
