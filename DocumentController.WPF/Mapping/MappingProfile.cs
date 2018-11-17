using AutoMapper;
using DocumentController.WPF.Models;
using DocumentController.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentController.WPF.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Document, DocumentViewModel>()
                .AfterMap((d, dvm) =>
                {
                    if (d.DocumentNumber == null) dvm.DocumentNumber = string.Empty;
                });
            CreateMap<DocumentVersion, DocumentVersionViewModel>()
                .AfterMap((dv, dvvm) =>
                {
                    if (dv.IsRemoved == null) dvvm.IsRemoved = "false";
                });

            CreateMap<DocumentViewModel, Document>();
            CreateMap<DocumentVersionViewModel, DocumentVersion>();
        }
    }
}
