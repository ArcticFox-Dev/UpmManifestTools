# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./UpmManifestTools.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Kazimierz Luska <kazimierz.luska@gmail.com>"
LABEL repository="https://github.com/ArcticFox-Dev"
LABEL homepage="https://github.com/ArcticFox-Dev/UpmManifestTools"

# Label as GitHub action
LABEL com.github.actions.name="Unity Package Manager Tools"
# Limit to 160 characters
LABEL com.github.actions.description="An action that will tag a package version with a snapshot and a timestamp."
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "/UpmManifestTools.dll" ]