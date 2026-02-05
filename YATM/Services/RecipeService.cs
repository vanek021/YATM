using AutoMapper;
using YATM.BlazorModels.Recipes;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Models.Entities.Recipes;

namespace YATM.Services
{
    public class RecipeService
    {
        private readonly ApplicationContext _appCtx;
        private readonly Database _db;
        private readonly IMapper _mapper;

        public RecipeService(ApplicationContext appCtx, Database db, IMapper mapper)
        {
            _appCtx = appCtx;
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<RecipeBlazorModel>> GetRecipesByUserAsync(User user)
        {
            var recipes = await _db.Recipes.GetAllByUserAsync(user.Id);
            return _mapper.Map<List<RecipeBlazorModel>>(recipes);
        }

        public async Task<Recipe> CreateRecipeAsync(RecipeBlazorModel model)
        {
            var recipe = _mapper.Map<Recipe>(model);
            recipe.UserId = _appCtx.CurrentUser.Id;
            _db.Recipes.Insert(recipe);
            await _db.SaveChangesAsync();
            return recipe;
        }

        public async Task UpdateRecipeAsync(RecipeBlazorModel model)
        {
            if (model.Id == default)
                throw new ArgumentException();

            var recipe = await _db.Recipes.GetByIdAsync(model.Id, _appCtx.CurrentUser.Id);

            if (recipe is null)
                return;

            _mapper.Map(model, recipe);
            _db.Recipes.Update(recipe);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(long id)
        {
            var recipe = await _db.Recipes.GetByIdAsync(id, _appCtx.CurrentUser.Id);

            if (recipe is null)
                return;

            _db.Recipes.Delete(recipe);
            await _db.SaveChangesAsync();
        }
    }
}
