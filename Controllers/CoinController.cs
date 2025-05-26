using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Interfaces;

namespace api.Controllers
{
    /// <summary>
    /// Coin işlemleri ile ilgili API uç noktalarını barındırır.
    /// Popüler coin'ler, tüm coin'ler ve istatistiksel veriler burada sunulur.
    /// </summary>
    [Route("api/coin")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        /// <summary>
        /// Popüler coin listesini getirir.
        /// </summary>
        /// <returns>Popüler coin DTO listesi.</returns>
        /// <response code="200">Liste başarıyla döndü.</response>
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularCoinModels()
        {
            var coinModelDtos = await _coinService.GetPopularCoinAsync();
            return Ok(coinModelDtos);
        }

        /// <summary>
        /// Sistemdeki tüm coin listesini döner.
        /// </summary>
        /// <returns>Tüm coin DTO listesi.</returns>
        /// <response code="200">Liste başarıyla döndü.</response>
        [HttpGet("All")]
        public async Task<IActionResult> GetAllCoinModels()
        {
            var coinModelDtos = await _coinService.GetAllAsync();
            return Ok(coinModelDtos);
        }

        /// <summary>
        /// Belirtilen coin'in son 7 günlük istatistik verilerini getirir.
        /// </summary>
        /// <param name="id">Coin ID (örnek: bitcoin, ethereum).</param>
        /// <returns>7 günlük tarihsel fiyat, market cap ve hacim verisi.</returns>
        /// <response code="200">Veriler başarıyla döndü.</response>
        /// <response code="400">ID boş veya geçersiz.</response>
        /// <response code="404">Veri bulunamadı.</response>
        [HttpGet("7daysstatistics")]
        public async Task<IActionResult> Get7DaysStatistics([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Coin ID gerekli.");
            }

            var chartData = await _coinService.Get7DaysStatistics(id);

            if (chartData == null)
            {
                return NotFound($"{id} için grafik verisi bulunamadı.");
            }

            return Ok(chartData);
        }

        /// <summary>
        /// Belirtilen coin'in son 15 günlük istatistik verilerini getirir.
        /// </summary>
        /// <param name="id">Coin ID (örnek: bitcoin, ethereum).</param>
        /// <returns>15 günlük tarihsel fiyat, market cap ve hacim verisi.</returns>
        /// <response code="200">Veriler başarıyla döndü.</response>
        /// <response code="400">ID boş veya geçersiz.</response>
        /// <response code="404">Veri bulunamadı.</response>
        [HttpGet("15daysstatistics")]
        public async Task<IActionResult> Get15DaysStatistics([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Coin ID gerekli.");
            }

            var chartData = await _coinService.Get15DaysStatistics(id);

            if (chartData == null)
            {
                return NotFound($"{id} için grafik verisi bulunamadı.");
            }

            return Ok(chartData);
        }

        /// <summary>
        /// Belirtilen coin'in son 30 günlük istatistik verilerini getirir.
        /// </summary>
        /// <param name="id">Coin ID (örnek: bitcoin, ethereum).</param>
        /// <returns>30 günlük tarihsel fiyat, market cap ve hacim verisi.</returns>
        /// <response code="200">Veriler başarıyla döndü.</response>
        /// <response code="400">ID boş veya geçersiz.</response>
        /// <response code="404">Veri bulunamadı.</response>
        [HttpGet("30daysstatistics")]
        public async Task<IActionResult> Get30DaysStatistics([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Coin ID gerekli.");
            }

            var chartData = await _coinService.Get30DaysStatistics(id);

            if (chartData == null)
            {
                return NotFound($"{id} için grafik verisi bulunamadı.");
            }

            return Ok(chartData);
        }
    }
}
