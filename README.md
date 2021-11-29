##  Projects
### Jelly.Threading.Channels.Queue

####    Features
1.  Implement In-Proc typed-queue base on `System.Threading.Channels`

####    Usage
1.  Implement your own typed-consumer base on `Jelly.Threading.Channels.Queue.QueueConsumerBase`, custom your own business logic by override `Handle(T data)`.
2.  Register typed-consumer at `ConfigureServices` of `Startup`.
3.  Configure channel by add `Queueoptions` at `ConfigureServices` of `Startup` for your needs.