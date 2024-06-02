using AutoMapper;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Extensions;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.RequestHelpers;

namespace YachtMarinaAPI.Services
{
    public interface IProductService
    {
        PagedList<Product> GetAll(ProductParams productParams);
        Task<Product> GetById(int productId);
        Task<int> Create(CreateProductDto dto);
        Task Update(UpdateProductDto dto);
        Task Delete(int productId);
        object GetFilters();
    }
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;

        public ProductService(ApplicationDbContext context, IMapper mapper, ImageService imageService,
            IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
        }

        public async Task<int> Create(CreateProductDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if (user.RoleId != 8)
            {
                throw new ForbidException("Nie masz uprawnień");
            }

            var product = _mapper.Map<Product>(dto);

            if (dto.File != null)
            {
                var imageResult = _imageService.AddImage(dto.File);

                if (imageResult.Result.Error != null)
                {
                    throw new BadRequestException("Cos poszlo nie tak z plikiem!");
                }

                product.PictureUrl = imageResult.Result.SecureUrl.ToString();

                product.PublicId = imageResult.Result.PublicId;

            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            var id = product.Id;

            return id;
        }

        public async Task Update(UpdateProductDto dto)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if ((user.RoleId != 7) && (user.RoleId != 8))
            {
                throw new ForbidException("Nie masz uprawnień");
            }

            var product = _context.Products.Find(dto.Id);

            if (product == null)
            {
                throw new NotFoundException("Nie znaleziono produktu");
            }

            _mapper.Map(dto, product);

            product.PictureUrl = product.PictureUrl;

            if (dto.File != null)
            {
                var imageResult = _imageService.AddImage(dto.File);


                if (!string.IsNullOrEmpty(product.PublicId))
                {
                    _imageService.DeleteImage(product.Id.ToString());
                }

                product.PictureUrl = imageResult.Result.SecureUrl.ToString();
                product.PublicId = imageResult.Result.PublicId;
            }

            await _context.SaveChangesAsync();

        }

        public async Task Delete(int productId)
        {
            var user = GetUser((int)_userContextService.LoggedUserId);

            if ( (user.RoleId != 7) && (user.RoleId != 8) )
            {
                throw new ForbidException("Nie masz uprawnień");
            }

            var product = _context.Products.Find(productId);

            if (product == null)
            {
                throw new NotFoundException("Nie znaleziono produktu");
            }

            //if (!string.IsNullOrEmpty(product.PublicId.ToString()))
            //{
                //_imageService.DeleteImage(product.Id.ToString());
            //}

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public PagedList<Product> GetAll(ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();


            var products = PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(products.MetaData);
            return products;
        }

        public async Task<Product> GetById(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException("Nie znaleziono takiego produktu");
            }

            return product;
        }

        public object GetFilters()
        {
            var brands = _context.Products
                .Select(p => p.Brand).Distinct().ToList();

            var types = _context.Products
                .Select(p => p.Type).Distinct().ToList();

            var result = new
            {
                Brands = brands,
                Types = types
            };

            return result;
        }

        private User GetUser(int id)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono użytkownika");
            }

            return user;
        }
    }
}
