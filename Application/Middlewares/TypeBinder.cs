using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace API.Middlewares;
public class TypeBinder <T> : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var propertyName = bindingContext.ModelName;
        var value = bindingContext.ValueProvider.GetValue(propertyName);

        if(value == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }
        else
        {
            if (value.FirstValue != null)
            {
                var deserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue); 
                bindingContext.Result = ModelBindingResult.Success(deserializedValue);

            }
            return Task.CompletedTask;
        }
    }
}
