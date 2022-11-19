using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using RestSharp;

namespace ACTech.CI.Services.CarEvaluation;

    public class CarEvaluationServiceOptions
    {
        public const string Root = "CarEvaluationService";
        public string Url { get; set; } = String.Empty;
    }
    public interface ICarEvaluationService  
    {  
        string WelcomeEquinox();
        CarModel[] GetModelsByManufacturer(int manufacturerId);

        Dictionary<int, string> Manufacturers {get;}
        Dictionary<int, string> BodyTypes {get;}
        Dictionary<int, string> FuelTypes {get;}
        Dictionary<int, string> TransmissionTypes {get;}
        Dictionary<int, string> DriveTypes {get;}
        Dictionary<string, decimal> EvaluateUsingIds(EvaluationModelBase evaluationModel);
        Dictionary<string, decimal> EvaluateUsingNames(EvaluationModelNames evaluationModel);
    }  
    public class CarEvaluationService : ICarEvaluationService
    {  
        private readonly CarEvaluationServiceOptions _config;
        readonly RestClient _client;


        public Dictionary<int, string> Manufacturers { get; private set; }
        private void SyncManufacturers()
        {
            Type manufacturerType = typeof(Dictionary<int, string>);
            Manufacturers = _client.GetJsonAsync<Dictionary<int, string>>("/make").Result;
        }
        private int GetManufacturerIdByName(string manufacturerName)
        {
            var res = Manufacturers.FirstOrDefault(x => x.Value == manufacturerName).Key;
            return res;
        }

        public Dictionary<int, string> BodyTypes { get; private set; }
        private void SyncBodyTypes()
        {
           BodyTypes = _client.GetJsonAsync<Dictionary<int, string>>("/body").Result;
        }
        private int GetBodyTypeIdByName(string bodyTypeName)
        {
            var res = BodyTypes.FirstOrDefault(x => x.Value == bodyTypeName).Key;
            return res;
        }        

        public Dictionary<int, string> FuelTypes { get; private set; }
        private void SyncFuelTypes()
        {
           FuelTypes = _client.GetJsonAsync<Dictionary<int, string>>("/fuel").Result;
        }
        private int GetFuelTypeIdByName(string FuelTypeName)
        {
            var res = FuelTypes.FirstOrDefault(x => x.Value == FuelTypeName).Key;
            return res;
        }

        public Dictionary<int, string> TransmissionTypes { get; private set; }
        private void SyncTransmissionTypes()
        {
           TransmissionTypes = _client.GetJsonAsync<Dictionary<int, string>>("/transmission").Result;
        }
        private int GetTransmissionTypeIdByName(string TransmissionTypeName)
        {
            var res = TransmissionTypes.FirstOrDefault(x => x.Value == TransmissionTypeName).Key;
            return res;
        }        

        public Dictionary<int, string> DriveTypes { get; private set; }
        private void SyncDriveTypes()
        {
           DriveTypes = _client.GetJsonAsync<Dictionary<int, string>>("/drive").Result;
        }
        private int GetDriveTypeIdByName(string DriveTypeName)
        {
            var res = DriveTypes.FirstOrDefault(x => x.Value == DriveTypeName).Key;
            return res;
        }

        public CarEvaluationService(
            ILogger<CarEvaluationService> logger,
            IConfiguration config

        )  
        {
            _config = new CarEvaluationServiceOptions();
            config.GetSection(CarEvaluationServiceOptions.Root).Bind(_config);

            var options = new RestClientOptions(_config.Url);
            _client = new RestClient(options);

            SyncManufacturers();
            SyncBodyTypes();
            SyncFuelTypes();
            SyncTransmissionTypes();
            SyncDriveTypes();
            SyncModels();

            //logger.LogInformation("test information.");
        }


        public Dictionary<int, CarModel> Models { get; private set; }
        private void SyncModels()
        {
            Models = new Dictionary<int, CarModel>();
            foreach (var manId in Manufacturers.Keys)
            {
                var manName = Manufacturers[manId];
                var models = GetModelsByManufacturerDirect(manId);
                foreach (var model in models)
                {
                    model.ManufacturerId = manId;
                    model.ManufacturerName = manName;
                    Models.Add(model.Id, model);
                }
            }
        }
        private CarModel GetCarModelById(int modelId)
        {
            var res = Models[modelId];
            return res;
        }

        private CarModel GetCarModelByManufacturerAndModelNames(string manufacturerName, string carModelName)
        {
            var res = Models.FirstOrDefault(x =>
                x.Value.ManufacturerName.ToLower() == manufacturerName.ToLower() &&
                x.Value.Name.ToLower() == carModelName.ToLower()
                ).Value;
            return res;
        }

        private CarModel[] GetModelsByManufacturerDirect(int manufacturerId)
        {
            var request = new RestRequest("/models")
                .AddQueryParameter("id", manufacturerId);

            var response = _client.GetAsync(request).Result;

            var content = JsonSerializer.Deserialize<CarModelData>(response.Content);
            return content.Data;
        }

        public CarModel[] GetModelsByManufacturer(int manufacturerId)
        {
            List<CarModel> models = new List<CarModel>();
            foreach (var modelentry in Models.Where(x => x.Value.ManufacturerId == manufacturerId))
            {
                models.Add(modelentry.Value);
            }
            return models.ToArray();
        }

        public Dictionary<string, decimal> EvaluateUsingIds(EvaluationModelBase evaluationModel)
        {
            var request = new RestRequest("/evaluate")
                .AddJsonBody<EvaluationModelBase>(evaluationModel);

            var response = _client.PostAsync<Dictionary<string, decimal>>(request).Result;
            return response;
        }

        public Dictionary<string, decimal> EvaluateUsingNames(EvaluationModelNames evaluationModel)
        {
            EvaluationModelBase idsBasedModel = new EvaluationModelBase() {
                Year = evaluationModel.Year,
                Mileage = evaluationModel.Mileage,
                EngineCapacity = evaluationModel.EngineCapacity,

                CarModel = GetCarModelByManufacturerAndModelNames(evaluationModel.ManufacturerName, evaluationModel.CarModelName).Id.ToString(),
                Fuel = GetFuelTypeIdByName(evaluationModel.Fuel).ToString(),
                Transmission = GetTransmissionTypeIdByName(evaluationModel.Transmission).ToString(),
                Drive = GetDriveTypeIdByName(evaluationModel.Drive).ToString(),
                Body = GetBodyTypeIdByName(evaluationModel.Body).ToString()
            };


            var request = new RestRequest("/evaluate")
                .AddJsonBody<EvaluationModelBase>(idsBasedModel);

            var response = _client.PostAsync<Dictionary<string, decimal>>(request).GetAwaiter().GetResult();
            return response;
        }


        public string welcomeEquinoxStr { get; set; }  
        public string WelcomeEquinox()  
        {  
            return welcomeEquinoxStr;
        }



    } 
