# TestDataDefinitionFramework
TestDataDefinitionFramework (or TDDF for short) is a library that abstracts the setup of test data from the implementation of data backing stores, so that the same test code can be run against both an in-memory/fake repository or a "real" repository using only pre-compiler conditionals to switch between the two.

An immediate design limitation is that this method will ONLY work when you are using "dumb data stores", so if you are running multi-table SQL queries or stored procedures, then this is not the library for you!

However, if your interactions with your data layer are usually that of "store this entity in this table", "query this entity from this table" then read on!

## The Use Case
Ordinarily when writing integration tests (for example using SpecFlow) you will decide; "is this going to run against an in-memory fake, or against the "real" repository".

There are pros and cons to deciding on one: "in-memory" is fast and can run on the build server, but doesn't thoroughly test your data provider layer. Using "real" repositories more thoroughly tests your code layers as it proves the integration of your code with the chosen data storage engine, but is slower and requires connectivity to a running instance of your data storage engine of choice (e.g. a MongoDB or SQL instance).

Ideally it's nice to have both options, but a lot times this leads to a duplication of the integration test code - in SpecFlow terms the "step definitions" can look very different when you want to "setup" a MongoDB than when you only want to setup an in-memory context and pass that to an interceptor/fake repository.

The idea of TDDF is that by setting your test data against the TestDataStore you can then use that same data in an in-memory repository as easily as enabling a "real" backing store and the TDDF plugins will take care of actually standing up the "real" data resource.

## Getting Started
* Clone the source code repository and build, or install packages via NuGet.
* Add a reference to "TestDataDefinitionFramework.Core" into your "integration tests" project (for example your SpecFlow/NUnit project).
* Choose which backing stores plugins you want to use (e.g. TestDataDefinitionFramework.MongoDB)
* Wire-up the code:

1) Configure and Initialize (do this before your tests run), e.g. in SpecFlow
```csharp
[BeforeTestRun]
public static async Task Initialize()
{
    var mongoBackingStore = new MongoBackingStore("ExampleSutDB");
    TestDataStore.AddRepository<SummaryItem>(cfg =>
    {
        cfg.WithName(SummaryCollection.Name);
#if UseRealProvider
            cfg.WithBackingStore(mongoBackingStore);
#endif
    });

    await TestDataStore.InitializeAllAsync();
}
```

2) Use the TestDataStore for your setting up your test data, e.g. in SpecFlow
```csharp
[Given(@"the summaries repository returns '(.*)'")]
public void GivenTheSummariesRepositoryReturns(IReadOnlyList<string> items)
{
    TestDataStore.Repository<SummaryItem>(SummaryCollection.Name).Items = items.Select(i => new SummaryItem {Name = i}).ToArray();
}
```

3) Add your in-memory repository fakes (for when running in-memory mode):
```csharp
public class WebTestFixture : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
#if !UseRealProvider
            services.AddTransient<ISummariesRepository, InMemorySummariesRepository>();
#endif
        });
    }
}
```

4) Implement your in-memory repository:
```csharp
public class InMemorySummariesRepository : ISummariesRepository
{
    public Task<IReadOnlyList<string>> GetAllAsync()
    {
        var result = TestDataStore.Repository<SummaryItem>(SummaryCollection.Name)
                            .Items?.Select(i => i.Name).ToArray()
            ?? Array.Empty<string>();

        return Task.FromResult((IReadOnlyList<string>) result);
    }
}
```

5) Commit the test data before calling the SUT (the best way to acheive this in SpecFlow is to trigger before the "When" block)
```csharp
[BeforeScenarioBlock]
public async Task Commit()
{
    if (_scenarioContext.CurrentScenarioBlock == ScenarioBlock.When)
    {
        await TestDataStore.CommitAllAsync();
    }
}
```

Now you can run your tests against an in-memory fake, or against a "real" repository by simply setting or unsetting a pre-compiler conditional called "UseRealProvider".

## Notes
* See the ExampleTests project for a working version of the above

## MongoDB Plugin Notes
* Provide your own connection string if you already have a MongoDB instance running
* If you don't provide a connection string, then the code will attempt to spin up a MongoDB instance on port 27017 using Docker Desktop (so this must be installed and that port must be free if you rely on this feature)
* The collections will be dropped and re-created on each commit, so please don't point this at a working MongoDB database!
* Make sure your "repository" names in TDDF match up with the collection name you use in the "real" repository

## Architecture
![Architecture Diagram](/docs/Architecture.png)

## Advanced Examples
### Using a custom builder
If you want to build up an object across multiple steps and then "build" it as part of the commit, then hook in earlier than the TDDF commit to set the state in the TestDataStore rather than having to remove the builder, e.g.:

```csharp
[Binding]
public class Context
{
    private readonly ScenarioContext _scenarioContext;

    public Context(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    public MyClassDataBuilder MyClassDataBuilder { get; set; }

    [BeforeScenarioBlock(Order = 0)]
    public void BeforeCommit()
    {
        if (_scenarioContext.CurrentScenarioBlock == ScenarioBlock.When)
        {
            var myClassInstance = MyClassDataBuilder?.Build();
            TestDataStore.Repository<MyClass>().Items =
                myClassInstance != null ?
                new[] { myClassInstance } :
                Array.Empty<MyClass>();
        }
    }
}
```

### Capturing provider calls with an interceptor when in "real" mode
Sometimes you'd like to capture calls that were made to your provider layer so that you can make assertions about what was called and with what data. Obviously by swapping out the interceptor to the "real" provider you lose this functionality (unless you could make the same assertion against the "real" repository, but that seems like a bigger problem).

The solution when using TDDF is *not* to remove your interceptor when switching to "real" mode, but instead to use the interceptor class as a decorator over the "real" implementation and inject the "real" class only when running in that mode. For example:

```csharp
protected override void ConfigureWebHost(IWebHostBuilder builder)
{
    base.ConfigureWebHost(builder);

    builder.UseEnvironment("Testing");

    builder.ConfigureTestServices(services =>
    {
    services.AddTransient<IMyDataStore, MyDataStoreInterceptor>(); // <-- always use the interceptor

#if UseRealProvider
    services.AddTransient<RealMyDataStore>(); // <-- when "real" mode, register the real implementation with .net DI
#endif
}

public class MyDataStoreInterceptor : IMyDataStore
{
    private readonly InterceptorsDataContext _interceptorsDataContext;
    private readonly RealMyDataStore _realDataStore;

    public EKycDataStoreInterceptor(InterceptorsDataContext interceptorsDataContext) // <-- this version is used when running in memory
     : this(interceptorsDataContext, null)
    {
        _interceptorsDataContext = interceptorsDataContext;
    }

    public EKycDataStoreInterceptor(InterceptorsDataContext interceptorsDataContext, RealMyDataStore realDataStore) // <-- this version is used when running "real" mode
    {
        _interceptorsDataContext = interceptorsDataContext;
        _realDataStore = realDataStore;
    }

    public Task StoreAsync(MyData myData)
    {
        _interceptorsDataContext.MyDataStoreContext.StoredData = myData;

        return _realDataStore != null ? 
            _realDataStore.StoreAsync(ekycData) : 
            Task.CompletedTask;
    }

    public Task<MyData> GetAsync(string reference)
    {
        return _realDataStore != null ? 
            _realDataStore.GetAsync(reference) :
            Task.FromResult(TestDataStore.Repository<MyData>().Items?.FirstOrDefault(i => i.Reference == reference))
    }
}
```

## Contributing
As you can see this repository is still in it's infancy and so far I've only needed to create a MongoDB plugin.
Feel free to create your own plugins and raise a merge request so this can grow in it's usefulness!
