using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Desafio3.Pages.Reports;

[Authorize]
public class IndexModel(IConfiguration configuration) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? MaxTiempo { get; set; }

    public string ReporteRecetasUrl => BuildReportUrl(
        "Reporte1_RecetasIngredientes",
        MaxTiempo.HasValue
            ? $"MaxTiempo={Uri.EscapeDataString(MaxTiempo.Value.ToString(CultureInfo.InvariantCulture))}"
            : null);

    public string ReportePasosUrl => BuildReportUrl("Reporte2_PasosReceta", null);

    private string BuildReportUrl(string reportName, string? parameter)
    {
        var reportServerUrl = configuration["Ssrs:ReportServerUrl"]?.TrimEnd('/')
            ?? throw new InvalidOperationException("Falta Ssrs:ReportServerUrl.");
        var reportPath = configuration["Ssrs:ReportPath"]?.Trim('/')
            ?? throw new InvalidOperationException("Falta Ssrs:ReportPath.");

        var reportUrl = $"{reportServerUrl}?/{reportPath}/{reportName}";
        var query = "rs:Command=Render&rs:Format=HTML5&rc:Toolbar=true";
        return string.IsNullOrWhiteSpace(parameter)
            ? $"{reportUrl}&{query}"
            : $"{reportUrl}&{query}&{parameter}";
    }
}
