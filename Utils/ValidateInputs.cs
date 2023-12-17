namespace ProductStore.Utils
{
    public class ValidateInputs
    {
        public static bool Validate(List<string> inputs)
        {
            foreach (var input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return true;
                }
            }
            return false;   
        }
    }
}
