# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DA.DinnerPlanner.Razor.Proto/DA.DinnerPlanner.Razor.Proto.csproj", "DA.DinnerPlanner.Razor.Proto/"]
COPY ["DA.DinnerPlanner.Common/DA.DinnerPlanner.Common.csproj", "DA.DinnerPlanner.Common/"]
COPY ["DA.DinnerPlanner.Model/DA.DinnerPlanner.Model.csproj", "DA.DinnerPlanner.Model/"]
RUN dotnet restore "./DA.DinnerPlanner.Razor.Proto/DA.DinnerPlanner.Razor.Proto.csproj"
COPY . .
WORKDIR "/src/DA.DinnerPlanner.Razor.Proto"
RUN dotnet build "./DA.DinnerPlanner.Razor.Proto.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DA.DinnerPlanner.Razor.Proto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DA.DinnerPlanner.Razor.Proto.dll"]