using Autodesk.Revit.DB;

using Bim4Everyone.RevitFiltration.Controls.Core;

namespace Bim4Everyone.RevitFiltration.Controls.ViewModels;

internal class CategoryViewModel : BaseViewModel, IEquatable<CategoryViewModel> {
    private readonly ElementId _id;
    private bool _isSelected;

    public CategoryViewModel(Category category) {
        Category = category ?? throw new ArgumentNullException(nameof(category));
        _id = Category.Id;
    }

    public Category Category { get; }

    public string Name => Category.Name;

    public bool IsSelected {
        get => _isSelected;
        set => RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public bool Equals(CategoryViewModel? other) {
        if(other is null) {
            return false;
        }

        if(ReferenceEquals(this, other)) {
            return true;
        }

        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj) {
        if(obj is null) {
            return false;
        }

        if(ReferenceEquals(this, obj)) {
            return true;
        }

        if(obj.GetType() != GetType()) {
            return false;
        }

        return Equals((CategoryViewModel) obj);
    }

    public override int GetHashCode() {
        return _id.GetHashCode();
    }
}
