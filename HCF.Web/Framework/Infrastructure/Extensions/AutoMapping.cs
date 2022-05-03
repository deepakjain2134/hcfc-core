using AutoMapper;
using HCF.BDO;
using HCF.Web.Areas.Api.Models.Request;
using HCF.Web.Areas.Api.Models.Response;
using HCF.Web.Models;

namespace HCF.Web.Framework.Infrastructure.Extensions
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<LoginModel, UserProfile>().ReverseMap();

            CreateMap<Assets, AssetViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AssetCode))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.AssetTypeId)).ReverseMap();

            //CreateMap<Assets, AssetRequestModel>().ReverseMap();

            CreateMap<AssetType, AssetTypeViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AssetTypeCode)).ReverseMap();

            //CreateMap<AssetType, AssetTypeRequestModel>().ReverseMap();

            CreateMap<WorkOrder, WorkOrderViewModel>().ReverseMap();
            CreateMap<SaveWorkOrderRequestModel, WorkOrder>().ReverseMap();
            CreateMap<UpdateWorkOrderRequestModel, WorkOrder>().ReverseMap();
            CreateMap<UpdateTempWorkOrderRequestModel, WorkOrder>().ReverseMap();
            CreateMap<WorkOrderFloorAssetsRequestModel, WorkOrderFloorAssets>().ReverseMap();

            CreateMap<UserProfile, LoginResponseModel>().ReverseMap();


            CreateMap<WoStatus, StatusResponseModel>().ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.WoStatusId)).ReverseMap();
            CreateMap<WoStatus, SaveStatusRequestModel>().ReverseMap();
            CreateMap<WoStatus, UpdateStatusRequestModel>().ReverseMap();


            CreateMap<SubStatus, SubStatusRepsonseModel>().ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.WoStatusId)).ReverseMap();
            CreateMap<SubStatus, SaveSubstatusRequestModel>().ReverseMap().ForMember(dest => dest.WoStatusId, opt => opt.MapFrom(src => src.StatusId));
            CreateMap<SubStatus, UpdateSubstatusRequestModel>().ReverseMap();

            CreateMap<Site, SiteResponseModel>().ForMember(dest => dest.SiteCode, opt => opt.MapFrom(src => src.Code)).ReverseMap();
            CreateMap<Site, SaveSiteRequestModel>().ReverseMap().ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SiteCode));
            CreateMap<Site, UpdateSiteRequestModel>().ReverseMap();

            CreateMap<Buildings, BuildingResponseModel>().ReverseMap();
            CreateMap<Buildings, SaveBuildingRequestModel>().ReverseMap();
            CreateMap<Buildings, UpdateBuildingRequestModel>().ReverseMap();


            CreateMap<Floor, FloorResponseModel>().ReverseMap();
            CreateMap<Floor, SaveFloorRequestModel>().ReverseMap();
            CreateMap<Floor, UpdateFloorRequestModel>().ReverseMap();

            CreateMap<UserProfile, UserResponseModel>().ReverseMap();
            CreateMap<UserProfile, SaveUserRequestModel>().ReverseMap();
            CreateMap<UserProfile, UpdateUserRequestModel>().ReverseMap();

            CreateMap<TFloorAssets, SaveFloorAssetRequestModel>().ReverseMap().ForMember(dest => dest.DeviceNo, opt => opt.MapFrom(src => src.AssetNo));
            CreateMap<TFloorAssets, UpdateFloorAssetRequestModel>().ReverseMap();
            CreateMap<TFloorAssets, FloorAssetViewModel>().ForMember(dest => dest.AssetNo, opt => opt.MapFrom(src => src.DeviceNo)).ForMember(dest => dest.CMMSReference, opt => opt.MapFrom(src => src.TmsReference)).ReverseMap();

            CreateMap<Assets, SubType>().ReverseMap();
            CreateMap<AssetType, TypesViewModel>().ReverseMap();


        }
    }
}
