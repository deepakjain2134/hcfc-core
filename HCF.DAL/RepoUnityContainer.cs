using HCF.BDO;
using HCF.DAL.Contexts;
using HCF.DAL.Interfaces;
using HCF.DAL.Repository;
using HCF.DAL.Repository.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCF.DAL
{
    public static class DalServiceCollectionExtensions
    {
        public static void AddRepositoryServices(this IServiceCollection container, IConfiguration configuration)
        {
            //container.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(
            //    configuration.GetConnectionString("DefaultConnection"),
            //    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            //container.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));


            // container.AddDbContext<HcfContext>(options => options.UseSqlServer());
            // container.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            container.AddScoped<ISqlHelper, SqlHelper>();
            container.AddScoped<ISqlConnection, SqlConnections>();

          

            container.AddScoped<IAssetsSubCategoryRepository, AssetsSubCategoryRepository>();
            container.AddScoped<IAssetTypeRepository, AssetTypeRepository>();

            container.AddScoped<IBuildVersionRepository, BuildVersionRepository>();
            container.AddScoped<IBuildingsRepository, BuildingsRepository>();


            container.AddScoped<ICategoryRepository, CategoryRepository>();
            container.AddScoped<IChartRepository, ChartRepository>();
            container.AddScoped<IConstructionRepository, ConstructionRepository>();

            container.AddScoped<IDeviceTestingRepository, DeviceTestingRepository>();
            container.AddScoped<IDepartmentRepository, DepartmentRepository>();
            container.AddScoped<IDigitalSignRepository, DigitalSignRepository>();
            container.AddScoped<IDocumentsRepository, DocumentsRepository>();


            container.AddScoped<IEPGroupsRepsitory, EPGroupsRepsitory>();
            container.AddScoped<IEmailRepository, EmailRepository>();

            container.AddScoped<IEPSchedulerRepository, EPSchedulerRepository>();
            container.AddScoped<IExercisesRepository, ExercisesRepository>();
            container.AddScoped<IEpDocumentRepository, EpDocumentRepository>();


            container.AddScoped<IFireExtinguisherRepository, FireExtinguisherRepository>();
            container.AddScoped<IFireWatchRepository, FireWatchRepository>();
            container.AddScoped<IFloorTypeRepository, FloorTypeRepository>();
            container.AddScoped<IFloorShapesRepository, FloorShapesRepository>();

            container.AddScoped<IGoalMaster, GoalMaster>();

            container.AddScoped<IHotWorkPermitRepository, HotWorkPermitRepository>();


            container.AddScoped<IIlsmRepository, IlsmRepository>();
            container.AddScoped<IInsStepsRepository, InsStepsRepository>();
            container.AddScoped<IInsStepsTaskRepository, InsStepsTaskRepository>();


            container.AddScoped<ILocationRepository, LocationRepository>();

            container.AddScoped<IManufactureRepository, ManufactureRepository>();
            container.AddScoped<IMenuRepository, MenuRepository>();

            container.AddScoped<INewsRepository, NewsRepository>();
            container.AddScoped<INotificationRepository, NotificationRepository>();

            container.AddScoped<IQuestionnairesRepository, QuestionnairesRepository>();

            container.AddScoped<IRolesRepository, RolesRepository>();
            container.AddScoped<IRoundInspectionsRepository, RoundInspectionsRepository>();
            container.AddScoped<IRounds, Rounds>();
            container.AddScoped<IRoundsQuesRepository, RoundsQuesRepository>();
            container.AddScoped<IReportsRepository, ReportsRepository>();


            container.AddScoped<ISiteRepository, SiteRepository>();
            container.AddScoped<IScheduleRepository, ScheduleRepository>();
            container.AddScoped<IShiftRepository, ShiftRepository>();
            container.AddScoped<IStatusRepository, StatusRepository>();
            container.AddScoped<IStandardRepository, StandardRepository>();
            container.AddScoped<ISkillsRepository, SkillsRepository>();
            container.AddScoped<ISearchRepository, SearchRepository>();
            container.AddScoped<ISubStatusRepository, SubStatusRepository>();

            container.AddScoped<ITIcraProjectRepository, TIcraProjectRepository>();
            container.AddScoped<ITypeRepository, TypeRepository>();
            container.AddScoped<ITipsRepository, TipsRepository>();
            container.AddScoped<ITFilesRepository, TFilesRepository>();
            container.AddScoped<ITEPInspectionRepository, TEPInspectionRepository>();


            container.AddScoped<IObservationReport, ObservationReport>();
            container.AddScoped<IOrganizationRepository, OrganizationRepository>();

            container.AddScoped<IPCRARepository, PCRARepository>();
            container.AddScoped<IPermitRepository, PermitRepository>();

            container.AddScoped<IVendorRepository, VendorRepository>();
            container.AddScoped<IWingRepository, WingRepository>();
            container.AddScoped<IPriorityRepository, PriorityRepository>();

            container.AddScoped<IUserSiteRepository, UserSiteRepository>();

            container.AddScoped<IAssetsRepository, AssetsRepository>();
            container.AddScoped<IBinderRepository, BinderRepository>();
            container.AddScoped<ICommonRepository, CommonRepository>();
            container.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            container.AddScoped<IEpRepository, EpRepository>();
            container.AddScoped<IFloorAssetRepository, FloorAssetRepository>();
            container.AddScoped<IFloorPlanRepository, FloorPlanRepository>();
            container.AddScoped<IFloorRepository, FloorRepository>();
            container.AddScoped<IFrequencyRepository, FrequencyRepository>();
            container.AddScoped<IGoalMaster, GoalMaster>();
            container.AddScoped<IInsDetailRepository, InsDetailRepository>();
            container.AddScoped<IInspectionActivityRepository, InspectionActivityRepository>();
            container.AddScoped<IInspectionRepository, InspectionRepository>();
            container.AddScoped<IMainGoalRepository, MainGoalRepository>();
            container.AddScoped<IStepsRepository, StepsRepository>();
            container.AddScoped<ITransaction, Transaction>();
            container.AddScoped<IUsersRepository, Users>();
            container.AddScoped<IUserLoginCodesRepository, UserLoginCodesRepository>();

            container.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            container.AddScoped<ITmsConnectorRepsitory, TmsConnectorRepsitory>();

            container.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();

            container.AddScoped<IUserLoginRepository, UserLoginRepository>();
            container.AddScoped<IActivityRepository, ActivityRepository>();
        }
    }  
    
}
