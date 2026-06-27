namespace Smart_Parking_Garage.Errors;

public static class UploadedFileErrors
{
    public static readonly Error EmptyImageFile =
      new("Images.Image is required.", "Image is required ", StatusCodes.Status400BadRequest);
    public static readonly Error ImageNotFound =
      new("Images.Image Not Found.", "ImageNotFound ", StatusCodes.Status400BadRequest);
}
