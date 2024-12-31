using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CrepaUrbana.Models;
using CrepaUrbana.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CrepaUrbana.Commands;

namespace CrepaUrbana.ViewModels
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService productoService;
        private Producto productoSeleccionado;

        public ObservableCollection<Producto> Productos { get; set; }

        public Producto ProductoSeleccionado
        {
            get { return productoSeleccionado; }
            set
            {
                productoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarProductoCommand { get; }
        public ICommand EditarProductoCommand { get; }
        public ICommand EliminarProductoCommand { get; }

        public ProductosViewModel()
        {
            productoService = new ProductoService();
            Productos = new ObservableCollection<Producto>(productoService.ObtenerProductos());

            AgregarProductoCommand = new RelayCommand(AgregarProducto);
            EditarProductoCommand = new RelayCommand(EditarProducto, PuedeEditarOEliminar);
            EliminarProductoCommand = new RelayCommand(EliminarProducto, PuedeEditarOEliminar);
        }

        private void AgregarProducto(object parameter)
        {
            var nuevoProducto = new Producto
            {
                Nombre = "Nuevo Producto",
                Precio = 0.0m,
                Categoria = "Categoría",
                Stock = 0
            };
            productoService.AgregarProducto(nuevoProducto);
            Productos.Add(nuevoProducto);
        }

        private void EditarProducto(object parameter)
        {
            if (ProductoSeleccionado != null)
            {
                productoService.ActualizarProducto(ProductoSeleccionado);
                // Actualizar la lista de productos si es necesario
            }
        }

        private void EliminarProducto(object parameter)
        {
            if (ProductoSeleccionado != null)
            {
                productoService.EliminarProducto(ProductoSeleccionado.IdProducto);
                Productos.Remove(ProductoSeleccionado);
            }
        }

        private bool PuedeEditarOEliminar(object parameter)
        {
            return ProductoSeleccionado != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

