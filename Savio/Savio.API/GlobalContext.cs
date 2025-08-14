using Savio.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User.Contract;
using User;
using Transaction.Contract;
using Category.Contract;
using Transaction;
using Category;

namespace Savio.API
{
    public class GlobalContext
    {
        public static IUserService UserService { get; }
        private static IUserRepository UserRepository { get; }
        public static ITransactionService TransactionService { get; }
        private static ITransactionRepository TransactionRepository { get; }
        public static ICategoryService CategoryService { get; }
        private static ICategoryRepository CategoryRepository { get; }

        static GlobalContext()
        {
            try
            {
                var configuration = GetConfig.Configuration;

                UserRepository = new UserRepository(configuration);
                UserService = new UserService(UserRepository);
                TransactionRepository = new TransactionRepository(configuration);
                TransactionService = new TransactionService(TransactionRepository);
                CategoryRepository = new CategoryRepository(configuration);
                CategoryService = new CategoryService(CategoryRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GlobalContext] Failed to initialize: " + ex.ToString());
                throw;
            }
        }
    }

}