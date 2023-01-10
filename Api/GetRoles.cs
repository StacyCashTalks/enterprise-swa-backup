using Newtonsoft.Json;

namespace Api
{
	public class GetRoles
	{
		private readonly ILogger _logger;

		public GetRoles(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<GetRoles>();
		}

		[Function("GetRoles")]
		public async Task<HttpResponseData> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post")]
			HttpRequestData request)
		{
			var content = await new StreamReader(request.Body).ReadToEndAsync();

			var clientPrincipal = JsonConvert.DeserializeObject<ClientPrincipal>(content);

			if (clientPrincipal == null)
			{
				var badResponse = request.CreateResponse(HttpStatusCode.NotFound);
				badResponse.WriteString("No principal supplied");

				return badResponse;
			}

			var roles = new List<string>
				{
					clientPrincipal.UserDetails.Replace(" ", "_"),
					clientPrincipal.IdentityProvider,
					"authorised"
				};

			var response = request.CreateResponse(HttpStatusCode.OK);
			await response.WriteAsJsonAsync(new
			{
				roles
			});

			return response;
		}
	}
}
