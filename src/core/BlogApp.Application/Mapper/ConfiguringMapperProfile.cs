using AutoMapper;
using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Domain.Models;

namespace BlogApp.Application.Mapper;

public class ConfiguringMapperProfile : Profile
{
    public ConfiguringMapperProfile()
    {
        CreateMap<PostResponse, Post>()
            .ReverseMap();

        CreateMap<PostUpdateRequest, Post>()
            .ReverseMap();
    }
}