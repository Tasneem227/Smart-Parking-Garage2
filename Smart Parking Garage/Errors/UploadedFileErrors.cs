namespace Smart_Parking_Garage.Errors;

public static class UploadedFileErrors
{
    public static readonly Error EmptyImageFile =
      new("Images.Image is required.", "Image is required ", StatusCodes.Status400BadRequest);

}
