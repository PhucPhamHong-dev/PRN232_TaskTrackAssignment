export type Department = { departmentId: number; departmentName: string; departmentDescription?: string; isActive?: boolean; projects?: Project[] };
export type Project = { projectId: number; projectName: string; description?: string; startDate: string; endDate?: string; status: number; departmentId: number; departmentName: string; isActive?: boolean; createdDate?: string; tasks?: WorkTask[] };
export type Tag = { tagId: number; tagName: string; color?: string };
export type WorkTask = { taskId: number; title: string; description?: string; status: number; priority: number; dueDate?: string; projectId: number; projectName: string; createdDate?: string; modifiedDate?: string; tags: Tag[] };

const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";

export async function api<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, { ...init, headers: { "Content-Type": "application/json", ...(init?.headers || {}) } });
  if (!response.ok) { const body = await response.json().catch(() => ({})); throw new Error(body.message || "Request failed"); }
  if (response.status === 204) return undefined as T;
  return response.json();
}

export const getDepartments = () => api<Department[]>("/api/departments");
export const getProjects = () => api<Project[]>("/api/projects");
export const getProject = (id: string) => api<Project>(`/api/projects/${id}`);
export const getDepartment = (id: string) => api<Department>(`/api/departments/${id}`);
export const getTask = (id: string) => api<WorkTask>(`/api/tasks/${id}`);
export const getTags = () => api<Tag[]>("/api/tags");
export const searchTasks = (query: string) => api<WorkTask[]>(`/api/tasks/search${query ? `?${query}` : ""}`);

export const statusLabels = ["Not Started", "In Progress", "Completed", "On Hold"];
export const taskStatusLabels = ["To Do", "In Progress", "Done", "Cancelled"];
export const priorityLabels = ["Low", "Medium", "High", "Critical"];
