FROM mcr.microsoft.com/dotnet/sdk:10.0

# Create a dedicated non-root user
RUN useradd \
    --create-home \
    --uid 2000 \
    --shell /bin/bash \
    flowai

# Create writable student workspace
RUN mkdir -p /workspace \
    && chown -R flowai:flowai /workspace

WORKDIR /workspace

# Student code runs as non-root
USER flowai