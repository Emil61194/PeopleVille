export async function GetSaveFiles() {
  const response = await fetch("http://localhost:5045/get/savefiles");
  if (!response.ok) {
    throw new Error("Unable to load save files");
  }

  const data = await response.json();
  if (!Array.isArray(data)) {
    throw new Error("Save files response was not an array");
  }

  return data;
}
