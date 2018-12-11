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
                .ForMember(d => d.Title, opt => opt.NullSubstitute(string.Empty))
                .ForMember(d => d.DocumentNumber, opt => opt.NullSubstitute(string.Empty));
            CreateMap<DocumentVersion, DocumentVersionViewModel>();
                //.ForMember(dvvm => dvvm.IsRemoved, opt => opt.NullSubstitute("false"));

            CreateMap<DocumentViewModel, Document>();
            CreateMap<DocumentVersionViewModel, DocumentVersion>();
        }
    }
}
