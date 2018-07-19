using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using Microsoft.SqlServer.Types;
using NUnit.Framework;

namespace EntityFramework.Metadata.Test.CodeFirst
{
    [TestFixture]
    public abstract class TestBase
    {
        protected const int NvarcharMax = 1073741823;
        [SetUp]
        public virtual void Setup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            SqlProviderServices.SqlServerTypesAssemblyName = typeof(SqlGeography).Assembly.FullName;

            if (!Database.Exists("TestContext"))
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<TestContext>());
            }
            else
            {
                Database.SetInitializer<TestContext>(null);
            }
        }

        protected TestContext GetContext()
        {
            var ctx = new TestContext();

            ctx.Configuration.AutoDetectChangesEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.ValidateOnSaveEnabled = false;

            return ctx;
        }

        protected void InitializeContext()
        {
            using (var ctx = GetContext())
            {
                var sw = new Stopwatch();
                sw.Start();
                var tmp = ctx.Pages.Count();
                sw.Stop();
                Console.WriteLine("Initializing dbmodel took: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
        }
    }
}