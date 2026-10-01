export interface SupportRequestTrackResponseModel {
  requestCode: string;
  subject: number;
  priority: number;
  title: string;
  description: string;
  status: number;
  userMessage: string | null;
}