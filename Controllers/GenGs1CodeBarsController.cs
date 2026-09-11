using Microsoft.AspNetCore.Mvc;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/v1/[controller]")]
public class GenGs1CodeBarsController : ControllerBase
{
    private readonly IBarcodeService _barCodeService;
    private readonly ILogger<GenGs1CodeBarsController> _logger;

    public GenGs1CodeBarsController(IBarcodeService barCodeService, ILogger<GenGs1CodeBarsController> logger)
    {
        _barCodeService = barCodeService;
        _logger = logger;
    }

    [HttpGet("{*codigo}")]
    [EnableRateLimiting("PorIp")]
    public IActionResult Get(string codigo)
    {
        try
        {
            var imagem = _barCodeService.GerarCodigoBarrasGs1(codigo);
            return File(imagem, "image/png");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
            "Etiqueta GS1 não foi gerada. Motivo: {Motivo}. Codigo recebido: {Codigo}",
            ex.Message,
            codigo);
            
            return BadRequest(ex.Message);
        }
    }
}