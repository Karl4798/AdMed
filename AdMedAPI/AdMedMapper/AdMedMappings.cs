using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AdMedAPI.Models;
using AdMedAPI.Models.Dtos;

namespace AdMedAPI.ParkyMapper
{
    public class AdMedMappings : Profile
    {

        public AdMedMappings()
        {
            CreateMap<Application, ApplicationCreateDto>().ReverseMap();
            CreateMap<Application, ApplicationUpdateDto>().ReverseMap();
            CreateMap<EmergencyContact, EmergencyContactCreateDto>().ReverseMap();
            CreateMap<EmergencyContact, EmergencyContactUpdateDto>().ReverseMap();

        }

    }
}
