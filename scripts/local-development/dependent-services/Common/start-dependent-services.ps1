docker run `
    --rm `
    -itd `
    -p 4000:5672 `
    -p 4100:15672 `
    -v eszop-servicebus-rabbitmq:/var/lib/rabbitmq `
    --hostname eszop-servicebus-rabbitmq `
    --network eszop-network `
    --name eszop-servicebus-rabbitmq `
    rabbitmq:3-management