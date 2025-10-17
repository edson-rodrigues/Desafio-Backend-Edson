using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.Interfaces;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.DeliveryDrivers;

public class UploadCNHImageCommandHandler : IRequestHandler<UploadCNHImageCommand, Result>
{
    private readonly IDeliveryDriverRepository _driverRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UploadCNHImageCommandHandler> _logger;

    private static readonly string[] AllowedExtensions = { ".png", ".bmp" };

    public UploadCNHImageCommandHandler(
        IDeliveryDriverRepository driverRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        ILogger<UploadCNHImageCommandHandler> logger)
    {
        _driverRepository = driverRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UploadCNHImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.DriverId, out var driverId))
            {
                return Result.Failure(Error.BadRequest("ID inválido"));
            }

            var driver = await _driverRepository.GetByIdAsync(driverId, cancellationToken);
            if (driver == null)
            {
                _logger.LogWarning("Driver not found: {DriverId}", driverId);
                return Result.Failure(Error.NotFound("DeliveryDriver", driverId));
            }

            // Validate file extension
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file extension for CNH image: {Extension}", extension);
                return Result.Failure(Error.BadRequest("Formato de arquivo inválido. Apenas PNG e BMP são permitidos"));
            }

            // Upload file
            var filePath = await _fileStorageService.UploadFileAsync(
                request.ImageStream,
                $"cnh/{driverId}/{Guid.NewGuid()}{extension}",
                request.ContentType,
                cancellationToken);

            // Update driver entity
            driver.UpdateCNHImage(filePath);

            await _driverRepository.UpdateAsync(driver, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("CNH image uploaded successfully for driver: {DriverId}", driverId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading CNH image");
            return Result.Failure(Error.Create("CNH_IMAGE_UPLOAD_FAILED", "Erro ao enviar imagem da CNH"));
        }
    }
}

