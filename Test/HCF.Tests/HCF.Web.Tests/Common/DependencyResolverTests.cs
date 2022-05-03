using HCF.BAL;
using HCF.Utility;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using HCF.Web.Base;
using Microsoft.AspNetCore.Http;
using HCF.DAL;
using HCF.BAL.Interfaces.Services;

namespace HCF.Tests.HCF.Web.Tests.Common
{
    [TestFixture]
    public class DependencyResolverTests : BaseTest
    {
        private DependencyResolverHelper _serviceProvider;

        public DependencyResolverTests()
        {

            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new DependencyResolverHelper(webHost);
        }

        [Test]
        public void Logger_Service_Should_Get_Resolved()
        {
            //Act
           // var YourService = _serviceProvider.GetService<ILogger>();

            //Assert
           /// Assert.IsNotNull(YourService);
        }

        [Test]
        public void AppSetting_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IAppSetting>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void Configuration_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IConfiguration>();
            //Assert   
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void HCFSession_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IHCFSession>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        //[Test]
        //public void CommonModel_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<ICommonModelFactory>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}

        [Test]
        public void CommonProvider_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<ICommonProvider>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void CookieUtilFactory_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<ICookieUtilFactory>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        //[Test]
        //public void ApiCommon_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<IApiCommon>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}

        [Test]
        public void MailService_Service_Should_Get_Resolved()
        {
            //Act
            //var YourService = _serviceProvider.GetService<IMailService>();
            ////Assert
            //Assert.IsNotNull(YourService);
        }

        [Test]
        public void EmailProcessor_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IEmailProcessor>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void HttpContextAccessor_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IHttpContextAccessor>();
            //Assert
            Assert.IsNotNull(YourService);
        }


        #region BAL Services 

        //[Test]
        //public void PCRAService_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<IPCRAService>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}
        //[Test]
        //public void Permit_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<IPermitService>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}

        //[Test]
        //public void HotWorkPermit_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<IHotWorkPermitService>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}

        [Test]
        public void ITIcraProject_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<ITIcraProjectService>();
            //Assert
            Assert.IsNotNull(YourService);
        }


        //[Test]
        //public void IConstructionService_Service_Should_Get_Resolved()
        //{
        //    //Act
        //    var YourService = _serviceProvider.GetService<IConstructionService>();
        //    //Assert
        //    Assert.IsNotNull(YourService);
        //}
        [Test]
        public void AssetsService_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IAssetsService>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        #endregion BAL Services 


        #region DAL Repository 

        [Test]
        public void PermitRepository_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IPermitRepository>();
            //Assert
            Assert.IsNotNull(YourService);
        }
        [Test]
        public void IPCRARepository_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IPCRARepository>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void TIcraProjectRepository_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<ITIcraProjectRepository>();
            //Assert
            Assert.IsNotNull(YourService);
        }

        [Test]
        public void ConstructionRepository_Service_Should_Get_Resolved()
        {
            //Act
            var YourService = _serviceProvider.GetService<IConstructionRepository>();
            //Assert
            Assert.IsNotNull(YourService);
        }
        #endregion DAL Repository 

    }
}
