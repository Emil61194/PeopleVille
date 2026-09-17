export async function GetSaveFiles() {
  //example api endpoint
  const url = "https://api.example.com/savefiles";
  const response = await fetch(url);
  const data = await response.json();
  // Process the data and return the list of save files
  return data.saveFiles; // Assuming the API returns an object with a 'saveFiles' property
}
