# Sử dụng hình ảnh SDK để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép các tệp csproj và khôi phục các gói NuGet
COPY ["KoiCareSystem.Api/KoiCareSystem.Api.csproj", "KoiCareSystem.Api/"]
COPY ["KoiCareSystem.Service/KoiCareSystem.Service.csproj", "KoiCareSystem.Service/"]
COPY ["KoiCareSystem.DataV1/KoiCareSystem.Data.csproj", "KoiCareSystem.DataV1/"]
COPY ["KoiCareSystem.Common/KoiCareSystem.Common.csproj", "KoiCareSystem.Common/"]

# Khôi phục các gói NuGet

RUN dotnet restore "KoiCareSystem.Api/KoiCareSystem.Api.csproj"

# Sao chép tất cả mã nguồn vào container
COPY . .

# Chuyển đến thư mục API để build
WORKDIR "/src/KoiCareSystem.Api"
RUN dotnet build "KoiCareSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KoiCareSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Tạo hình ảnh chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KoiCareSystem.Api.dll"]
