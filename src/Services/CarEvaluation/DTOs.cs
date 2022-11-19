using System.Text.Json.Serialization;



namespace ACTech.CI.Services.CarEvaluation;

public class EvaluationResult
{
    [JsonPropertyName("year")]
    public int Year {get; set;}
    [JsonPropertyName("price")]
    public decimal Price {get; set;}
}

public class CarModel
{
    [JsonPropertyName("id")]
    public int Id {get; set;}

    [JsonPropertyName("value")]
    public string Name {get; set;}

    [JsonPropertyName("body_type")]
    public string[] BodyType {get; set;}

    [JsonPropertyName("drive")]
    public string[] Drive {get; set;}

    [JsonPropertyName("engine_size")]
    public decimal[] EngineSize {get; set;}

    [JsonPropertyName("fuel_type")]
    public string[] FuelType {get; set;}

    [JsonPropertyName("transmission")]
    public string[] Transmission {get; set;}

    public int ManufacturerId;
    public string ManufacturerName;
}

public class CarModelData
{
    [JsonPropertyName("data")]
    public CarModel[] Data {get; set;}
}

public class EvaluationModelBase
{
    [JsonPropertyName("model")]
    public string CarModel {get; set;}

    [JsonPropertyName("year")]
    public string Year {get; set;}

    [JsonPropertyName("mileage")]
    public string Mileage {get; set;}

    [JsonPropertyName("engine")]
    public string EngineCapacity {get; set;}

    [JsonPropertyName("fuel")]
    public string Fuel {get; set;}

    [JsonPropertyName("transmission")]
    public string Transmission {get; set;}

    [JsonPropertyName("drive")]
    public string Drive {get; set;}

    [JsonPropertyName("body")]
    public string Body {get; set;}
}


public class EvaluationModelNames
{
    [JsonPropertyName("manufacturer")]
    public string ManufacturerName {get; set;}

    [JsonPropertyName("model")]
    public string CarModelName {get; set;}

    [JsonPropertyName("year")]
    public string Year {get; set;}

    [JsonPropertyName("mileage")]
    public string Mileage {get; set;}

    [JsonPropertyName("engine")]
    public string EngineCapacity {get; set;}

    [JsonPropertyName("fuel")]
    public string Fuel {get; set;}

    [JsonPropertyName("transmission")]
    public string Transmission {get; set;}

    [JsonPropertyName("drive")]
    public string Drive {get; set;}

    [JsonPropertyName("body")]
    public string Body {get; set;}
}

