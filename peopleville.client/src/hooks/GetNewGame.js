export async function GetNewGame() {
  const response = await fetch("http://localhost:5045/get/savefiles/create");
  if (!response.ok) {
    throw new Error("Failed to create new game save file");
  }
  return true;
}
