using ContosoPizza.Data;
using ContosoPizza.Business;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ServerDB");

//Scopeds
//Pizza
builder.Services.AddScoped<IPizzaService, PizzaService>(); 

// builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<IPizzaRepository, PizzaSqlRepository>(serviceProvider => 
    new PizzaSqlRepository(connectionString));

//Order
builder.Services.AddScoped<IOrderService, OrderService>(); 

builder.Services.AddScoped<IOrderRepository, OrderSqlRepository>(serviceProvider => 
    new OrderSqlRepository(connectionString));

// builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//User
builder.Services.AddScoped<IUserService, UserService>(); 

builder.Services.AddScoped<IUserRepository, UserSqlRepository>(serviceProvider => 
    new UserSqlRepository(connectionString));

// builder.Services.AddScoped<IUserRepository, UserRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    //mirar para que el isdevelopment lo pille en docker
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
