using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using Webion.Templates.Cli.Commands;
using Microsoft.Extensions.Configuration;
using Webion.Templates.Http.Extensions;

var app = new CommandLineBuilder(new TemplateCommand())
    .UseHost(builder =>
    {
        builder.ConfigureAppConfiguration((context, config) =>
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        );

        builder.ConfigureServices((ctx, services) =>
        {
            services.AddTemplatesClient(
                ctx.Configuration["TemplatesOptions:Url"] 
                    ?? throw new ArgumentNullException()
            );
        });

        builder.UseCommandHandler<ListCommand, ListCommand.Handler>();
        builder.UseCommandHandler<ShowCommand, ShowCommand.Handler>();
        builder.UseCommandHandler<DeleteCommand, DeleteCommand.Handler>();
        builder.UseCommandHandler<AddCommand, AddCommand.Handler>();
        builder.UseCommandHandler<EditCommand, EditCommand.Handler>();
        builder.UseCommandHandler<ProcessCommand, ProcessCommand.Handler>();
    })
    .UseDefaults()
    .Build();

await app.InvokeAsync(args);