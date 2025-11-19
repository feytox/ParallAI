using System.Text.Json;
using System.Text.Json.Serialization;

namespace ParallAI.Infrastructure.Util;

public class JsonWebEnumConverter<T>() : JsonStringEnumConverter<T>(JsonNamingPolicy.CamelCase) where T : struct, Enum;