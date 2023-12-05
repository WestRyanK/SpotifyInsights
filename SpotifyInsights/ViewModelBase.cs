using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotifyInsights;

internal class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if ((field == null ^ value == null) ||
			(field != null && !field.Equals(value)))
		{

			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
		return false;
	}

}
