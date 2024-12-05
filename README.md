# QueueLoadLeveling
## Queue Load Leveling Pattern

## Cài đặt

1. Clone

```bash
git clone https://github.com/dangxlam/QueueLoadLeveling.git
```
2. Set up
 ```bash
- Consumer/local.settings.json: điền azureservicebus accesskey vào "ServiceBusConnectionString": ""
- Producer/config.json: điền azureservicebus accesskey vào "ServiceBusConnectionString": ""
```

3. Send Message

```bash
cd Producer
dotnet run
```
4. Receive Message
```bash
cd Consumer
func start
```

## Contributing
Welcome
