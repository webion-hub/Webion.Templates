using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Webion.Templates.Cli.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Webion.Templates.Cli.Options;
using Webion.Templates.Cli.Commands;
using Webion.Templates.Cli.Abstraction;

var app = new CommandLineBuilder(new TemplateCommand())
    .UseHost(builder =>
    {
        builder.ConfigureServices((ctx, services) =>
        {
            services.Configure<TemplatesOptions>(config =>
            {
                config.Url = "https://templates-api-p6erbd2ysa-nw.a.run.app";
            });

            services.AddSingleton<TemplatesClient>();
            services.AddHttpClient<ITemplatesClient, TemplatesClient>();
        });

        builder.UseCommandHandler<ListCommand, ListCommand.Handler>();
        builder.UseCommandHandler<ShowCommand, ShowCommand.Handler>();
        builder.UseCommandHandler<DeleteCommand, DeleteCommand.Handler>();
        builder.UseCommandHandler<AddCommand, AddCommand.Handler>();
        builder.UseCommandHandler<EditCommand, EditCommand.Handler>();
    })
    .UseDefaults()
    .Build();

await app.InvokeAsync(args);