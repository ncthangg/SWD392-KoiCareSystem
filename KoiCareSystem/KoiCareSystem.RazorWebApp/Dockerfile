FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["KoiCareSystem.RazorWebApp/KoiCareSystem.RazorWebApp.csproj", "KoiCareSystem.RazorWebApp/"]
COPY ["KoiCareSystem.Service/KoiCareSystem.Service.csproj", "KoiCareSystem.Service/"]
COPY ["KoiCareSystem.Data/KoiCareSystem.Data.csproj", "KoiCareSystem.Data/"]
COPY ["KoiCareSystem.Common/KoiCareSystem.Common.csproj", "KoiCareSystem.Common/"]

# Khôi phục các gói NuGet
RUN dotnet restore "KoiCareSystem.RazorWebApp/KoiCareSystem.RazorWebApp.csproj"

# Sao chép tất cả mã nguồn vào container
COPY . .

# Chuyển đến thư mục RazorWebApp để build
WORKDIR "/src/KoiCareSystem.RazorWebApp"
RUN dotnet build "KoiCareSystem.RazorWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KoiCareSystem.RazorWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "KoiCareSystem.RazorWebApp.dll"]



