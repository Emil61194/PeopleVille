const MAX_ATTEMPTS = 5;
const RETRY_DELAY_MS = 1500;

export async function GetNewGame() {
  let lastError;

  for (let attempt = 1; attempt <= MAX_ATTEMPTS; attempt++) {
    try {
      const response = await fetch("/get/savefiles/create");
      if (response.ok) {
        return true;
      }
      lastError = new Error(
        `Failed to create new game save file (HTTP ${response.status})`,
      );
    } catch (error) {
      lastError = error;
    }

    if (attempt < MAX_ATTEMPTS) {
      await new Promise((resolve) => setTimeout(resolve, RETRY_DELAY_MS));
    }
  }

  throw lastError ?? new Error("Failed to create new game save file");
}