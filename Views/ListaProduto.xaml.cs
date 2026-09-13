using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();
    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = Lista;
    }

    protected async override void OnAppearing()
    {
        try
        {


            List<Produto> tmp = await App.Db.GetAll();

            Lista.Clear();

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }


    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            Lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private async void txt_categoria_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string categoria = e.NewTextValue;

            Lista.Clear();

            if (string.IsNullOrWhiteSpace(categoria))
            {
                List<Produto> tmp = await App.Db.GetAll();

                tmp.ForEach(i => Lista.Add(i));
            }
            else
            {
                List<Produto> tmp = await App.Db.SearchCategoria(categoria);

                tmp.ForEach(i => Lista.Add(i));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(Object sender, EventArgs e)
    {
        double soma = Lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if(confirm)
            {
                await App.Db.Delete(p.Id);
                Lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
        }
    private async void ToolbarItem_Relatorio(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCategoria());
    }
}
