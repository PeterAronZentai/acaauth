ASPNETCORE_URLS=http://localhost:3456 dapr run \
    --app-id svc1 \
    --app-port 3456 \
    --dapr-http-port 3501 \
    -- \
    dotnet run --no-launch-profile