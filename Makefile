include .env
export

run-product-dapr:
	dapr run \
    --app-id product-api \
    --app-port 5001 \
    --resources-path components \
    --config components/daprConfig.yaml \
    -- dotnet run --project src/Product/TodoApp.Product.API/TodoApp.Product.API.csprj && \
	cd -
.PHONY: run-product-dapr