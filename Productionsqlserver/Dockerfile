
# ──────────────────────────────────────────────────────────────────────────────
# 1) Build Stage (actually SQL Server Linux images are already “runnable”,
#    so we don’t need a multi‐stage build—just pull the official image)
# ──────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/mssql/server:2019-latest

# Accept the EULA and set the SA password (we’ll override the password via an ENV var)
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD="ChangeMeBeforeDeploy!"  

# Expose the default SQL Server port
EXPOSE 1433

# Create a directory where the database files will live
USER root
RUN mkdir -p /var/opt/mssql/data
RUN chown mssql /var/opt/mssql/data

VOLUME [ "/var/opt/mssql" ]  
# (Render will mount a persistent disk at /var/opt/mssql so your data survives restarts)

# Switch back to the ‘mssql’ user and run SQL Server
USER mssql
ENTRYPOINT [ "/opt/mssql/bin/sqlservr" ]