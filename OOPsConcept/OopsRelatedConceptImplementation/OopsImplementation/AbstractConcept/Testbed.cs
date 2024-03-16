namespace OopsRelatedConceptImplementation.OopsImplementation.AbstractConcept;

class Testbed
{
    /*public static void Call(string methodName, Action<Animal> methodToInvoke, Animal instance, object[] parameters)
    {
        try
        {
            // Get the method info based on the provided method name
            var methodInfo = instance.GetType().GetMethod(methodName);

            if (methodInfo != null)
            {
                // Invoke the method with the provided parameters
                methodInfo.Invoke(instance, parameters);
            }
            else
            {
                Console.WriteLine($"Method '{methodName}' not found in the provided instance.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }*/

    public static void Call(string methodName, Action<Animal> methodToInvoke, params Animal[] instances)
    {
        foreach (var instance in instances)
        {
            try
            {
                // Get the method info based on the provided method name
                var methodInfo = instance.GetType().GetMethod(methodName);

                if (methodInfo != null)
                {
                    // Invoke the method
                    methodInfo.Invoke(instance, null);
                }
                else
                {
                    Console.WriteLine($"Method '{methodName}' not found in the provided instance.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
