namespace HCF.BAL.Ioc
{

    using HCF.DAL;
    using HCF.BAL.Interfaces.Services;
    using HCF.BAL.Interfaces;
    using HCF.BAL.Services;
    using Microsoft.Extensions.DependencyInjection;
    using HCF.BAL.Services.JobService;
    using Microsoft.Extensions.Configuration;
using HCF.DAL.Interfaces;
using HCF.DAL.Repository.Users;

    public static class BAlServiceCollectionExtensions
    {
        public static void AddRepositoryServices(this IServiceCollection container, IConfiguration configuration)
        {
            //container.AddDbContext<HcfContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //container.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            DalServiceCollectionExtensions.AddRepositoryServices(container, configuration);

            //container.AddScoped<IAssemblyProvider, AssemblyProvider>();
            container.AddScoped<IActivityService, ActivityService>();
            container.AddScoped<IAssetsService, AssetsService>();
            container.AddScoped<IAssetTypeService, AssetTypeService>();

            container.AddScoped<IBinderService, BinderService>();
            container.AddScoped<IBuildingsService, BuildingsService>();
            container.AddScoped<IBuildVersionService, BuildVersionService>();

            container.AddScoped<ICategoryService, CategoryService>();
            container.AddScoped<IChartService, ChartService>();
            container.AddScoped<ICommonService, CommonService>();
            container.AddScoped<IConstructionService, ConstructionService>();
            container.AddScoped<ICacheService, CacheService>();

            container.AddScoped<IDepartmentService, DepartmentService>();
            container.AddScoped<IDocumentTypeService, DocumentTypeService>();
            container.AddScoped<IDocumentsService, DocumentsService>();
            container.AddScoped<IDigitalSignService, DigitalSignService>();
            container.AddScoped<IDeviceTestingService, DeviceTestingService>();

            container.AddScoped<IEpService, EpService>();
            container.AddScoped<IEPSchedulerService, EPSchedulerService>();
            container.AddScoped<IEpGroupsService, EpGroupsService>();
            container.AddScoped<IEmailService, EmailService>();
            container.AddScoped<IExercisesService, ExercisesService>();

            container.AddScoped<IInspectionService, InspectionService>();
            container.AddScoped<IInsActivityService, InsActivityService>();
            container.AddScoped<IInsStepsService, InsStepsService>();
            container.AddScoped<IIlsmService, IlsmService>();


            //container.AddScoped<IFireDrillService, FireDrillService>();
            container.AddScoped<IFloorService, FloorService>();
            container.AddScoped<IFloorTypeService, FloorTypeService>();
            container.AddScoped<IFireWatchService, FireWatchService>();
            container.AddScoped<IFloorAssetService, FloorAssetService>();
            container.AddScoped<IFrequencyService, FrequencyService>();
            container.AddScoped<IFireExtinguisherService, FireExtinguisherService>();
            container.AddScoped<IFloorShapesService, FloorShapesService>();

            container.AddScoped<IGoalMasterService, GoalMasterService>();

            container.AddScoped<IHotWorkPermitService, HotWorkPermitService>();




            container.AddScoped<ILoggingService, LoggingService>();
            container.AddScoped<ILocationService, LocationService>();

            container.AddScoped<IMainGoalService, MainGoalService>();
            container.AddScoped<IMenuService, MenuService>();
            container.AddScoped<IManufactureService, ManufactureService>();

            container.AddScoped<INewsService, NewsService>();
            container.AddScoped<INotificationService, NotificationService>();
            container.AddScoped<INewsService, NewsService>();

            container.AddScoped<IOrganizationService, OrganizationService>();

            container.AddScoped<IPriorityService, PriorityService>();
            container.AddScoped<IPCRAService, PCRAService>();
            container.AddScoped<IPermitService, PermitService>();
            container.AddScoped<IPdfService, PdfService>();

            container.AddScoped<IQuestionnairesService, QuestionnairesService>();

            container.AddScoped<IReportsService, ReportsService>();
            container.AddScoped<IRolesService, RolesService>();
            container.AddScoped<IRoundInspectionsService, RoundInspectionsService>();
            container.AddScoped<IRoundsService, RoundsService>();

            container.AddScoped<ISearchService, SearchService>();
            container.AddScoped<ISkillsService, SkillsService>();
            container.AddScoped<IStandardService, StandardService>();
            container.AddScoped<ISubStatusService, SubStatusService>();
            container.AddScoped<IStepsService, StepsService>();
            container.AddScoped<IShiftService, ShiftService>();
            container.AddScoped<IStatusService, StatusService>();
            container.AddScoped<IScheduleService, ScheduleService>();

            container.AddScoped<ITipsService, TipsService>();
            container.AddScoped<ITransactionService, TransactionService>();
            container.AddScoped<ITypeService, TypeService>();
            container.AddScoped<ITIcraProjectService, TIcraProjectService>();

            container.AddScoped<IUserSiteService, UserSiteService>();
            container.AddScoped<IUserService, UserService>();
            container.AddScoped<ISiteService, SiteService>();
            container.AddScoped<IUserEpService, UserEpService>();
            //  container.AddScoped<IUserLoginService, UserLoginService>();
            container.AddScoped<IUserLoginCodesService, UserLoginCodesService>();
            container.AddScoped<IVendorService, VendorService>();


            container.AddScoped<IWingService, WingService>();
            container.AddScoped<IWorkOrderService, WorkOrderService>();
            container.AddScoped<ITEPInspectionService, TEPInspectionService>();
            container.AddScoped<ITmsConnectorService, TmsConnectorService>();
            container.AddScoped<IRoundRecuringJobService, RoundRecuringJobService>();

            container.AddScoped<ITrainingSessionService, TrainingSessionService>();

        }
    }
}