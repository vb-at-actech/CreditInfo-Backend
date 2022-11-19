using Microsoft.AspNetCore.Mvc;
using ACTech.CI.Services.CarEvaluation;

namespace ACTech.CI.PublicAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CarEvaluationController : ControllerBase
{
    private readonly ILogger<CarEvaluationController> _logger;

    public CarEvaluationController(ILogger<CarEvaluationController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetManufacturers")]
    public Dictionary<int, string> GetManufacturers([FromServices] ICarEvaluationService ceService)
    {
        var mans = ceService.Manufacturers;
        return mans;
    }

    [HttpGet(Name = "GetModelsByManufacturer")]
    public CarModel[] GetModelsByManufacturer([FromServices] ICarEvaluationService ceService, int manufacturerId)
    {
        var models = ceService.GetModelsByManufacturer(manufacturerId);
        return models;
    }


    [HttpGet(Name = "GetBodyTypes")]
    public Dictionary<int, string> GetBodyTypes([FromServices] ICarEvaluationService ceService)
    {
        var bts = ceService.BodyTypes;
        return bts;
    }

    [HttpGet(Name = "GetFuelTypes")]
    public Dictionary<int, string> GetFuelTypes([FromServices] ICarEvaluationService ceService)
    {
        var fts = ceService.FuelTypes;
        return fts;
    }

    [HttpGet(Name = "GetTransmissionTypes")]
    public Dictionary<int, string> GetTransmissionTypes([FromServices] ICarEvaluationService ceService)
    {
        var tts = ceService.TransmissionTypes;
        return tts;
    }

    [HttpGet(Name = "GetDriveTypes")]
    public Dictionary<int, string> GetDriveTypes([FromServices] ICarEvaluationService ceService)
    {
        var dts = ceService.DriveTypes;
        return dts;
    }


    [HttpPost(Name = "EvaluateUsingIds")]
    public IActionResult EvaluateUsingIds([FromServices] ICarEvaluationService ceService, EvaluationModelBase evaluationModel)
    {
        var evaluationResult = ceService.EvaluateUsingIds(evaluationModel);
        return Ok(evaluationResult);
    }

    [HttpPost(Name = "EvaluateUsingNames")]
    public IActionResult EvaluateUsingNames([FromServices] ICarEvaluationService ceService, EvaluationModelNames evaluationModel)
    {
        var evaluationResult = ceService.EvaluateUsingNames(evaluationModel);
        return Ok(evaluationResult);
    }


}
