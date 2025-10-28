using AICore;
using AICore.Entities;
using Infrastructure.Entities;
using Mapster;

namespace Infrastructure;

public static class MappingConfigurator
{
    public static TypeAdapterConfig ConfigureMappings()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<UserDto, User>()
            .ConstructUsing(dto => new User(dto.Id))
            .TwoWays();

        config.NewConfig<AiModelDto, AiModel>()
            .ConstructUsing(dto => new AiModel(dto.Id, dto.Name))
            .TwoWays();
        
        return config;
    }
}